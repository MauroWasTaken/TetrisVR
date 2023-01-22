using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TetriminoScript : MonoBehaviour
{
    [SerializeField]
    private List<Material> tetriminoMaterials = new List<Material>();

    [SerializeField]
    private GameObject cubePrefab;

    private List<GameObject> cubes = new List<GameObject>();

    [SerializeField]
    private float fallSpeed = 0.5f;

    //custom fixed update to circumvent unity's fixed update
    private float fixedUpdateTime;

    private float fixedUpdateTimer = 0f;

    //declare a 4x7 array of Vector2
    private Vector2[,]
        cubeLocations =
            new Vector2[7,
            4]
            {
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(0, 2),
                    new Vector2(0, 3)
                }, //line
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 0),
                    new Vector2(1, 1)
                }, //square
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(0, 2),
                    new Vector2(1, 2)
                }, //L
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(0, 2),
                    new Vector2(-1, 2)
                }, //J
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(2, 0),
                    new Vector2(1, 1)
                }, //T
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(2, 1)
                }, //S
                {
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(2, 0)
                } //Z
            };
    [SerializeField]
    private int currentTetrimino = -1;

    private int nextTetrimino = -1;

    private Vector3 startingPoint;

    private bool canHold = true;

    private int heldTetrimino = -1;

    private GameManagerScript gameManager;

    void Start()
    {
        Time.timeScale = 1f;
        startingPoint = transform.position;
        SpawnTetrimino();
        gameManager = GameManagerScript.instance;
        gameManager.UpdateHUD();
        SubscribeToEvents();
    }
    
    private void SubscribeToEvents()
    {
        gameManager.OnLevelChanged.AddListener(UpdateFallSpeed);
    }

    private void UpdateFallSpeed()
    {
        fallSpeed -= 0.05f;;
    }
    
    private void SpawnTetrimino(bool isSwap = false)
    {
        cubes.Clear();
        fixedUpdateTime = fallSpeed;
        transform.position = startingPoint;
        //if isSwap is false, generate a new number for next tetrimino
        if (!isSwap)
        {
            if (nextTetrimino == -1)
            {
                currentTetrimino = Random.Range(0, tetriminoMaterials.Count);
            }
            else
            {
                currentTetrimino = nextTetrimino;
            }
            nextTetrimino = Random.Range(0, tetriminoMaterials.Count);
            PreviewTetrimino(nextTetrimino, new Vector3(12, 10, 20), 8);
        }
        //spawn the tetrimino
        Material tetriminoMaterial = tetriminoMaterials[currentTetrimino];
        CheckSpawn();
        for (int i = 0; i < 4; i++)
        {
            GameObject cube =
                Instantiate(cubePrefab,
                new Vector3(cubeLocations[currentTetrimino, i].x +
                    transform.position.x,
                    cubeLocations[currentTetrimino, i].y + transform.position.y,
                    transform.position.z),
                Quaternion.identity);
            cube.transform.parent = transform;
            cube.GetComponent<Renderer>().material = tetriminoMaterial;
            cubes.Add (cube);
        }
    }

    private void PreviewTetrimino(int tetriminoId, Vector3 position, int layer)
    {
        Material tetriminoMaterial = tetriminoMaterials[tetriminoId];
        var cubes =
            GameObject
                .FindGameObjectsWithTag("Cube")
                .Where(x => x.transform.gameObject.layer == layer);
        foreach (GameObject cube in cubes)
        {
            Destroy (cube);
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject cube =
                Instantiate(cubePrefab,
                new Vector3(cubeLocations[tetriminoId, i].x + position.x,
                    cubeLocations[tetriminoId, i].y + position.y,
                    position.z),
                Quaternion.identity);
            cube.GetComponent<Renderer>().material = tetriminoMaterial;
            cube.GetComponent<Collider>().enabled = false;
            cube.layer = layer;
        }
    }

    private void CheckSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 cubePosition =
                new Vector3(cubeLocations[currentTetrimino, i].x +
                    transform.position.x,
                    cubeLocations[currentTetrimino, i].y + transform.position.y,
                    transform.position.z);
            Collider[] colliders =
                Physics
                    .OverlapSphere(cubePosition, 0.2f)
                    .Where(x => x.transform.gameObject.layer == 7)
                    .ToArray();
            if (colliders.Length > 0)
            {
                gameManager.GameOver();
            }
        }
    }

    private void CheckLines(){
        bool result = false;
        int nbLines = -1;
        do{
            nbLines++;
            result = CheckLine();
        }while(result);
        if(nbLines > 0){
            gameManager.AddLines(nbLines);
        }
    }
    
    private bool CheckLine(){
        for (int i = 0; i < 20; i++)
        {
            GameObject[] cubes =
                GameObject
                    .FindGameObjectsWithTag("Cube")
                    .Where(x =>
                        x.transform.position.y == i &&
                        x.transform.gameObject.layer == 7)
                    .ToArray();
            if (cubes.Length >= 10)
            {
                foreach (GameObject cube in cubes)
                {
                    cube.SetActive(false);
                    Destroy (cube);
                }
                MoveDown(i);
                return true;
            }
        }
        return false;
    }
    private void MoveDown(int line){
        GameObject.FindGameObjectsWithTag("Cube").Where(x => x.transform.position.y > line && x.transform.gameObject.layer == 7).ToList().ForEach(x => x.transform.position += Vector3.down);
    }

    private void PlaceTetrimino()
    {
        for (int i = 0; i < 4; i++)
        {
            cubes[i].transform.parent = null;
            cubes[i].GetComponent<CubeScript>().enabled = false;
            cubes[i].layer = 7;
            cubes[i].transform.position =
                new Vector3(cubes[i].transform.position.x,
                    Mathf.Round(cubes[i].transform.position.y),
                    cubes[i].transform.position.z);
            cubes[i].GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeAll;
        }
        CheckLines();
        canHold = true;
    }

    void Update()
    {
        CheckUserInput();
        fixedUpdateTimer += Time.deltaTime;
        if (fixedUpdateTimer >= fixedUpdateTime)
        {
            fixedUpdateTimer = 0f;
            if (IsMoveAvailable(Vector3.down))
            {
                transform.position += Vector3.down;
            }
            else
            {
                PlaceTetrimino();
                SpawnTetrimino();
            }
        }
    }
    bool horizontalReturnedToZero = true;
    void CheckUserInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontalReturnedToZero && Mathf.Abs(0-horizontal)>0.75f)
        {
            MovePiece(Mathf.RoundToInt(horizontal));
            horizontalReturnedToZero = false;
        }else{
            if(Mathf.Abs(0-horizontal)<0.75f){
                horizontalReturnedToZero = true;
            }
        }
        if (Input.GetButtonDown("Rotate"))
        {
            Rotate(Input.GetAxis("Rotate") > 0);
        }

        // keyboard hard and soft drop
        if (Input.GetButtonDown("HardDrop"))
        {
            HardDrop();
        }
        if (Input.GetButtonDown("SoftDrop"))
        {
            fixedUpdateTime = 0.02f;
            fixedUpdateTimer = 0f;
        }
        if (Input.GetButtonUp("SoftDrop"))
        {
            fixedUpdateTime = fallSpeed;
        }
        if (Input.GetButtonDown("Hold"))
        {
            HoldTetramino();
        }

        if(Input.GetKey(KeyCode.D)&Input.GetKey(KeyCode.I)&Input.GetKey(KeyCode.O)){
            gameManager.ToggleEasterEgg();
        }
    }

    public void HardDrop(){
        while (IsMoveAvailable(Vector3.down))
        {
            transform.position += Vector3.down;
        }
        PlaceTetrimino();
        SpawnTetrimino();
    }
    public void HoldTetramino()
    {
        if (canHold)
        {
            var cubes =
                GameObject
                    .FindGameObjectsWithTag("Cube")
                    .Where(x => x.transform.gameObject.layer == 6);
            foreach (GameObject cube in cubes)
            {
                Destroy (cube);
            }
            if (heldTetrimino == -1)
            {
                heldTetrimino = currentTetrimino;
                SpawnTetrimino(false);
            }
            else
            {
                int temp = currentTetrimino;
                currentTetrimino = heldTetrimino;
                heldTetrimino = temp;
                SpawnTetrimino(true);
            }
            PreviewTetrimino(heldTetrimino, new Vector3(-10, 10, 20), 9);
            canHold = false;
        }
    }

    bool IsMoveAvailable(Vector3 offset)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 cubePosition = cubes[i].transform.position + offset;
            Collider[] colliders =
                Physics
                    .OverlapSphere(cubePosition, 0.2f)
                    .Where(x => x.transform.gameObject.layer == 7)
                    .ToArray();
            if (colliders.Length > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void MovePiece(int direction)
    {
        if (IsMoveAvailable(new Vector3(direction, 0, 0)))
        {
            transform.position += new Vector3(direction, 0, 0);
        }
    }

    bool IsRotationAvailable(bool isClockwise, Vector3 offset, Vector3 averagePosition)
    {
        for (int i = 0; i < 4; i++)
        {
           Vector3 newPos = GetRotationCoordenates(cubes[i].transform.position,isClockwise,averagePosition);
            Vector3 cubePosition =
                new (newPos.x + offset.x, newPos.y + offset.y, cubes[i].transform.position.z);
            Collider[] colliders =
                Physics
                    .OverlapSphere(cubePosition, 0.2f)
                    .Where(x => x.transform.gameObject.layer == 7)
                    .ToArray();
            if (colliders.Length > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void Rotate(bool isClockwise)
    {
        if (currentTetrimino == 1)
        {
            return;
        }
        Vector3 averagePosition =
            new Vector3(cubes.Average(cube => cube.transform.position.x),
                Mathf.Floor(cubes.Average(cube => cube.transform.position.y)),
                cubes.Average(cube => cube.transform.position.z));


        if(averagePosition.x % 1 == 0.5f){
            averagePosition.x = Mathf.Round(averagePosition.x);
        }
        float xOffset = 0;
        if(currentTetrimino >= 2 & currentTetrimino <= 3){
            if(!isClockwise){
                xOffset = 1f;
            }
        }
        if(currentTetrimino==4){
            if(!isClockwise){
                averagePosition.x = Mathf.Ceil(averagePosition.x);
            }
            // float newPos = GetRotationCoordenates(cubes[i].transform.position,isClockwise,averagePosition).Average(cube => cube.transform.position.x);
            // if(newPos % 1 == 0.25f){
            //     xOffset = 1f;
            // }else if(newPos % 1 == 0.75f)
        }
        Vector3 offset = new Vector3(0+xOffset, 0, 0);
        if (!IsRotationAvailable(isClockwise,offset,averagePosition))
        {
            offset = new Vector3(1+xOffset, 0, 0);
            if (!IsRotationAvailable(isClockwise,offset,averagePosition))
            {
                offset = new Vector3(-1+xOffset, 0, 0);
                if (!IsRotationAvailable(isClockwise,offset,averagePosition))
                {
                    offset = new Vector3(0+xOffset, -1, 0);
                    if (!IsRotationAvailable(isClockwise,offset,averagePosition))
                    {
                        return;
                    }
                      
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            Vector3 newPos = GetRotationCoordenates(cubes[i].transform.position,isClockwise,averagePosition);
            cubes[i].transform.position =
                new Vector3(newPos.x + offset.x, newPos.y + offset.y, cubes[i].transform.position.z);
        }
    }
    private Vector3 GetRotationCoordenates (Vector3 input,bool isClockwise,Vector3 averagePosition){
        float newY, newX;
        if (isClockwise)
        {
            newY =
                    (
                    input.x - averagePosition.x
                    ) *
                    -1 +
                    averagePosition.y;
            newX =
                    input.y -
                    averagePosition.y +
                    averagePosition.x;
        }
        else
        {
            newY =
                    input.x -
                    averagePosition.x +
                    averagePosition.y;
            newX =
                    (input.y - averagePosition.y) *
                    -1 +
                    averagePosition.x;
        }
        
        if(isClockwise){
            newY = Mathf.Floor(newY);
            newX = Mathf.Floor(newX);
        }
        else{
            newY = Mathf.Floor(newY);
            newX = Mathf.Floor(newX);
        }

        return new Vector3(newX, newY, input.z);
    }
}
