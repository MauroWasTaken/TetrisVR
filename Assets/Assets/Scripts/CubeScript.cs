using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    private void OnDestroy() {
        if(this.gameObject.layer==7){
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<Renderer>().material=GetComponent<MeshRenderer>().material;
            Destroy(effect, 1.0f);
        }
    }
}
