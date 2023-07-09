using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MultiplayerGame : MonoBehaviour
{
    public TMP_Text lifeDisplay;
    public GameObject playerModel;
    public GameObject enemyModel;
    private Dictionary<string, PlayerObject> players = new Dictionary<string, PlayerObject>();
    private Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();

    private class PlayerObject {
        public GameObject gameObject;
        public bool isAttacking;
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
                        Debug.Log("running animation");
                        var animationTime = time.Value - currentTime;
                        Debug.Log(animationTime);
                        animator.SetFloat("AttackSpeed", 350 / animationTime);
                        animator.SetTrigger("Attack");
                    }
                } else {
                    playerObject.isAttacking = false;
                }

                if (player.Key == SocketManager.user.username) {
                    lifeDisplay.text = string.Concat(Enumerable.Repeat("S2", player.Value.health));
                }
            }

            foreach (var enemy in objects.enemies) {
                var enemyObject = enemies.GetOrAdd(enemy.Key, () => Instantiate(enemyModel));
                enemyObject.transform.position = new Vector3(enemy.Value.position.x, enemy.Value.position.y);
            }
       });

       SocketManager.Socket.OnUnityThread("enemyHurt", (response) => {
            var id = response.GetValue<int>();
            var spriteRenderer = enemies[id].GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            StartCoroutine(ResetSprite(spriteRenderer));
       });

       SocketManager.Socket.OnUnityThread("levelWon", (response) => {
            response.CallbackAsync("maxHealth");
       });

       SocketManager.Socket.OnUnityThread("gameOver", (response) => {
            GameOver.response = response;
            SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
       });
    }

    IEnumerator ResetSprite(SpriteRenderer spriteRenderer) {
        yield return new WaitForSeconds(1);

        spriteRenderer.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
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
