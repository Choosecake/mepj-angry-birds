using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("slingshot")]
    [SerializeField] private GameObject slingshotHolder;
    [HideInInspector] public int _currentSlingshot = 0;
    private GameObject _slingshotObject;
    
    [Header("bird")]
    [SerializeField] private GameObject birdHolder;
    [HideInInspector] public int _currentBird = 0;
    private GameObject _birdObject;
    
    private void Update()
    {
        _slingshotObject = slingshotHolder.transform.GetChild(_currentSlingshot).gameObject;
        _birdObject = birdHolder.transform.GetChild(_currentBird).gameObject;
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_slingshotObject.GetComponent<Slingshot>().birdWasShot) return;
            
            slingshotHolder.transform.GetChild(_currentSlingshot).gameObject.SetActive(false);
            
            if (_currentSlingshot < slingshotHolder.transform.childCount - 1)
            {
                _currentSlingshot++;
            }
            else
            {
                _currentSlingshot = 0;
            }
            
            slingshotHolder.transform.GetChild(_currentSlingshot).gameObject.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_slingshotObject.GetComponent<Slingshot>().birdWasShot) return;
            
            birdHolder.transform.GetChild(_currentBird).gameObject.SetActive(false);
            
            if (_currentBird < birdHolder.transform.childCount - 1)
            {
                _currentBird++;
            }
            else
            {
                _currentBird = 0;
            }
            
            birdHolder.transform.GetChild(_currentBird).gameObject.SetActive(true);
        }
    }
}