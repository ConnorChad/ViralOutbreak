using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera cam;
    public LayerMask layer;
    private bool paused;
    public GameObject pauseMenu;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Debug.Log(cam.ScreenPointToRay(Input.mousePosition));
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
          
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
               
                Debug.DrawRay(ray.origin, hit.point, Color.yellow);
                Debug.Log(hit.transform.name);
                NonInfected script;
                script = hit.transform.gameObject.GetComponent<NonInfected>();
                script.infected = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else if (!paused)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }
}
