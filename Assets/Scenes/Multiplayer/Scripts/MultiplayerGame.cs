using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiplayerGame : MonoBehaviour
{
    public GameObject playerModel;
    public GameObject enemyModel;
    public GameObject coinModel;
    public GameObject heartModel;
    public Transform heartPosition;
    public GameObject shoper;
    public GameObject civilian;
    public GameObject dialogueBox;
    public GameObject optionsBox;
    public GameObject shop;

    private Dictionary<string, PlayerObject> players = new Dictionary<string, PlayerObject>();
    private Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> coins = new Dictionary<int, GameObject>();
    private List<GameObject> hearts = new List<GameObject>();
    private bool canMove = true;

    private class PlayerObject {
        public GameObject gameObject;
        public bool isAttacking;
        public bool canMove;
    }

    [Serializable]
    public class Position {
        public float x;
        public float y;
    }

    [Serializable]
    public class Player {
        public Position position;
        public int health;
        public string facing;
        public bool invincible;
        public long? endAttackTime;
    }

    [Serializable]
    public class Enemy {
        public Position position;
        public int health;
    }

    [Serializable]
    public class Objects {
        public Dictionary<string, Player> players;
        public Dictionary<int, Enemy> enemies;
        public Dictionary<int, Position> coins;
        public Position shoper;
        public Position mission;
    }

    // Start is called before the first frame update
    void Start()
    {
       SocketManager.Socket.OnUnityThread("objects", (response) => {
            var objects = response.GetValue<Objects>();

            foreach (var player in objects.players) {
                var playerObject = players.GetOrAdd(player.Key, () => {
                    GameObject gameObject;
                    if (player.Key == SocketManager.user.username) {
                        gameObject = playerModel;
                    } else {
                        gameObject = Instantiate(playerModel);
                    }

                    gameObject.SetActive(true);

                    return new PlayerObject {
                        gameObject = gameObject,
                        isAttacking = false,
                    };
                });
                var oldPos = playerObject.gameObject.transform.position;

                playerObject.gameObject.transform.position = new Vector3(player.Value.position.x, player.Value.position.y);

                int z = -1;
                if (player.Value.facing == "up") {
                    z = 270;
                } else if (player.Value.facing == "down") {
                    z = 90;
                } else if (player.Value.facing == "left") {
                    z = 180;
                } else if (player.Value.facing == "right") {
                    z = 0;
                }
                playerObject.gameObject.transform.rotation = Quaternion.Euler(0, 0, z);

                var animator = playerObject.gameObject.GetComponentInChildren<Animator>();
                if (playerObject.gameObject.transform.position == oldPos) {
                    animator.SetTrigger("AndandoFalse");
                } else {
                    animator.SetTrigger("AndandoTrue");
                }

                var time = player.Value.endAttackTime;
                DateTime currentDateTime = DateTime.UtcNow;
                long currentTime = (long)(currentDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                if (time.HasValue) {
                    if (!playerObject.isAttacking) {
                        playerObject.isAttacking = true;
                        var animationTime = time.Value - currentTime;
                        animator.SetFloat("AttackSpeed", 350 / animationTime);
                        animator.SetTrigger("Attack");
                    }
                } else {
                    playerObject.isAttacking = false;
                }

                if (player.Key == SocketManager.user.username) {
                    for (int i = hearts.Count - 1; i >= player.Value.health; i--) {
                        DestroyImmediate(hearts[i]);
                        hearts.RemoveAt(i);
                    }

                    Vector3 spawnPosition = new Vector3(heartPosition.position.x + hearts.Count * 35, heartPosition.position.y, 0);

                    for (int i = 0; i < player.Value.health - hearts.Count; i++) {
                        var heart = Instantiate(heartModel, spawnPosition, Quaternion.identity);
                        heart.transform.SetParent(heartPosition);
                        spawnPosition.x += 35;
                        hearts.Add(heart);
                    }
                }
            }

            foreach (var enemy in objects.enemies) {
                var enemyObject = enemies.GetOrAdd(enemy.Key, () => Instantiate(enemyModel));
                enemyObject.transform.position = new Vector3(enemy.Value.position.x, enemy.Value.position.y);
            }

            foreach (var coin in objects.coins) {
                var coinObject = coins.GetOrAdd(coin.Key, () => Instantiate(coinModel));
                coinObject.transform.position = new Vector3(coin.Value.x, coin.Value.y);
            }

            if (objects.shoper != null) {
                var position = objects.shoper;
                shoper.transform.position = new Vector3(position.x, position.y, 0);
                shoper.SetActive(true);
            } else {
                shoper.SetActive(false);
            }

            if (objects.mission != null) {
                var position = objects.mission;
                civilian.transform.position = new Vector3(position.x, position.y, 0);
                civilian.SetActive(true);
            } else {
                civilian.SetActive(false);
            }
       });

       SocketManager.Socket.OnUnityThread("playerHurt", (response) => {
            var username = response.GetValue<string>();
            var spriteRenderer = players[username].gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            StartCoroutine(ResetSprite(spriteRenderer));
       });

       SocketManager.Socket.OnUnityThread("enemyHurt", (response) => {
            var id = response.GetValue<int>();
            var spriteRenderer = enemies[id].GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            StartCoroutine(ResetSprite(spriteRenderer));
       });

       SocketManager.Socket.OnUnityThread("enemyKilled", (response) => {
            var id = response.GetValue<int>();
            var enemy = enemies[id];
            enemies.Remove(id);
            Destroy(enemy);
       });

       SocketManager.Socket.OnUnityThread("mission", (response) => {
            canMove = false;

            var monsters = response.GetValue<int>();
            ShowDialogue(new[] {
                "HELP!!!! There are " + monsters + " monsters attacking me!!!",
                "I think they are stronger than last time...",
            }, () => {
                ShowOptions(new []{
                    "Help him now",
                    "Just a moment"
                }, (option) => {
                    if (option == 0) {
                        response.CallbackAsync("start");
                    } else {
                        response.CallbackAsync("cancel");
                    }
                    canMove = true;
                });
            });
       });

       SocketManager.Socket.OnUnityThread("shop", (response) => {
            canMove = false;
            Shop.response = response;
            Shop.onClose = () => {
                canMove = true;
            };
            shop.SetActive(true);
       });

       SocketManager.Socket.OnUnityThread("coinCollected", (response) => {
            var id = response.GetValue<int>();
            var coin = coins[id];
            coins.Remove(id);
            Destroy(coin);
       });

       SocketManager.Socket.OnUnityThread("endLevel", (response) => {
            canMove = false;
            ShowDialogue(new[] {
                "THANK YOU!!",
                "I think we're safe for now",
            }, () => { canMove = true; });
       });

       SocketManager.Socket.OnUnityThread("gameOver", (response) => {
            GameOver.response = response;
            SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
       });
    }

    IEnumerator ResetSprite(SpriteRenderer spriteRenderer) {
        yield return new WaitForSeconds(1);

        if (spriteRenderer != null) {
            spriteRenderer.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            var movement = new Position {
              x = x,
              y = y
            };

            SocketManager.Socket.Emit("move", movement);

            if (Input.GetKeyDown(KeyCode.Space)) {
                SocketManager.Socket.Emit("attack");
            }
        }
    }

    void ShowDialogue(string[] text, Action onFinish = null) {
        Dialogue.text = text;
        Dialogue.onFinish = onFinish;
        dialogueBox.SetActive(true);
    }

    void ShowOptions(string[] options, Action<int> onPress = null) {
        Options.options = options;
        Options.onPress = onPress;
        optionsBox.SetActive(true);
    }
}
