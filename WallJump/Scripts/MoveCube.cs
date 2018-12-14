using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;

public class MoveCube : MonoBehaviour
{


    public GameObject goal;
    public GameObject ground;
    public Material goalScoredMaterial;
    public GameObject spawnArea;
    public Material groundMaterial;

    public Text roundText;
    public Text gameOverText;
    public Button restartButton;

    public float moveSpeed;
    public int rotationSpeed;

    Bounds spawnAreaBounds;

    Renderer groundRenderer;
    private Rigidbody rigidbody;
    private Animator animator;
    Vector3 moveDirection = Vector3.zero;

    Vector3[] randomPositions = {
        new Vector3(38.14f,-0.1f,57.07f),
        new Vector3(9.82f,-0.1f,57.07f),
        new Vector3(5.92f,-0.1f,57.07f),
        new Vector3(1.56f,-0.01f,57.07f),
        new Vector3(-6.89f,-0.01f,57.53f),
        new Vector3(-6.89f,-0.01f,37.8f),
        new Vector3(-7.78f,-0.01f,25.94f),
        new Vector3(-7.78f,-0.01f,11.93f),
        new Vector3(-2.44f,-0.01f,11.93f),
        new Vector3(11.35f,-0.01f,11.93f),
        new Vector3(37.07f,-0.01f,11.93f),
        new Vector3(37.07f,-0.01f,29.65f),
        new Vector3(37.07f,-0.01f,46.25f),
        new Vector3(37.07f,-0.01f,52.62f),
        new Vector3(37.07f,-0.01f,57.07f)
        };

    private int positionIndex=0;
    // Use this for initialization


    public GameObject AI;

    void Awake()
    {

        restartButton.onClick.AddListener(Restart);
        // Set the dafault value of gameover text to be false
        gameOverText.enabled = false;


        groundRenderer = ground.GetComponent<Renderer>();
        //groundMaterial = ground.GetComponent<Material>();
        spawnAreaBounds = spawnArea.GetComponent<Collider>().bounds;
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Reset();

        //print(restartButton.enabled);





    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * rotationSpeed);

        if (Input.GetKey(KeyCode.W))
            rigidbody.AddRelativeForce(Vector3.forward * moveSpeed);
        else if (Input.GetKey(KeyCode.S))
            rigidbody.AddRelativeForce(Vector3.forward * -moveSpeed);
        else if (Input.GetKey(KeyCode.A))
            rigidbody.AddRelativeForce(Vector3.left * moveSpeed);
        else if (Input.GetKey(KeyCode.D))
            rigidbody.AddRelativeForce(Vector3.right * moveSpeed);
    }
    

    void OnTriggerStay(Collider col)
    {        
        if (col.gameObject.CompareTag("goal"))
        {
            StartCoroutine(GoalScoredSwapGroundMaterial(goalScoredMaterial, 2));

            Reset();
        }
    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        groundRenderer.material = mat;
        yield return new WaitForSeconds(time); //wait for 2 sec
        groundRenderer.material = groundMaterial;
    }

    /// <summary>
    /// Gets a random spawn position in the spawningArea.
    /// </summary>
    /// <returns>The random spawn position.</returns>
    public Vector3 GetRandomSpawnPos()
    {

        //bool foundNewSpawnLocation = false;
        //Vector3 randomSpawnPos = Vector3.zero;
        //while (foundNewSpawnLocation == false)
        //{
        //    float randomPosX = Random.Range(-spawnAreaBounds.extents.x, spawnAreaBounds.extents.x);

        //    float randomPosZ = Random.Range(-spawnAreaBounds.extents.z, spawnAreaBounds.extents.z);

        //    Debug.Log("spawnArea.transform.position:" + spawnArea.transform.position.x.ToString() + " " + spawnArea.transform.position.y.ToString() + " " + spawnArea.transform.position.z.ToString());

        //    randomSpawnPos = spawnArea.transform.position + new Vector3(randomPosX, 1f, randomPosZ);

        //    //Vector3 halfExtends = new Vector3((),1.5f, GetComponent<Collider>().bounds / 2);

        //    if (Physics.CheckBox(randomSpawnPos, GetComponent<Collider>().bounds.extents / 2) == false)
        //    {

        //        foundNewSpawnLocation = true;
        //        Debug.Log("NO Collision");
        //        Debug.Log("randomPosX:" + randomPosX);
        //        Debug.Log("randomPosY:" + randomSpawnPos.y);
        //        Debug.Log("randomPosZ:" + randomPosZ);
        //        Debug.Log("=========================");
        //    }

        //}
        Vector3 randomSpawnPos = Vector3.zero;
        if (positionIndex == randomPositions.Length)
        //if (positionIndex == 1)
        {

            GameOver(0);
            positionIndex = 0;
        }

        randomSpawnPos = spawnArea.transform.position - randomPositions[positionIndex];
        //Debug.Log("randomSpawnPos:" + randomSpawnPos.x + " " + randomSpawnPos.y + " " + randomSpawnPos.z);
        positionIndex++;

        return randomSpawnPos;
    }

    private void Reset()
    {

        roundText.text = "Player Round: " + (positionIndex + 1).ToString();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.localPosition = GetRandomSpawnPos();
        //Debug.Log("transform.position:" + transform.position.x + " " + transform.position.y + " " + transform.position.z);
        //transform.Rotate(new Vector3(0, 0, 0));




    }

    public void GameOver(int whoWin)
    {

        //print(whoWin);

        // whoWin == 1 means AI Win
        if (whoWin == 1){
            gameOverText.text = "AI Win!!";
        }
        AI.SendMessage("AIGameOver");
        gameOverText.enabled = true;
        this.enabled = false;

    }

    void Restart(){
        this.positionIndex = 0;
        this.enabled = true;
        this.Awake();
        AI.SendMessage("AIRestart");

    }
    
}
