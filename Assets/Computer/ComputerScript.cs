using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComputerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    private bool interactable = true;

    public Animator animator;
    public Image image;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && interactable){
            StartCoroutine(TurnOn());
        }
    }

    private IEnumerator TurnOn()
    {
        var on = Instantiate(prefab, transform.position, transform.rotation);
        GetComponent<MeshRenderer>().enabled = false;
        interactable = false;

        yield return new WaitForSeconds(2);
        Destroy(on, 2);
        GetComponent<MeshRenderer>().enabled = true;
        interactable = true;

        animator.SetBool("fade", true);
        yield return new WaitUntil(() => image.color.a==1);
        SceneManager.LoadScene("ComputerScreen");
    }




}
