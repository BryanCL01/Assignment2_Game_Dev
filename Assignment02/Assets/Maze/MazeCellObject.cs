using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] GameObject topWall; 
    [SerializeField] GameObject bottomWall; 
    [SerializeField] GameObject rightWall; 
    [SerializeField] GameObject leftWall; 
    [SerializeField] GameObject floor; 

    //For materials
    [SerializeField] Material NorthWallMaterial;
    [SerializeField] Material SouthWallMaterial;
    [SerializeField] Material EastWallMaterial;
    [SerializeField] Material WestWallMaterial;
    [SerializeField] Material floorMaterial;
   

    public void Init (bool top, bool bottom, bool right, bool left) {
        topWall.SetActive(top); 
        bottomWall.SetActive(bottom); 
        rightWall.SetActive(right); 
        leftWall.SetActive(left); 


        if (topWall != null)
            topWall.GetComponent<Renderer>().material = NorthWallMaterial;
        if (bottomWall != null)
            bottomWall.GetComponent<Renderer>().material = SouthWallMaterial;
        if (rightWall != null)
            rightWall.GetComponent<Renderer>().material = EastWallMaterial;
        if (leftWall != null)
            leftWall.GetComponent<Renderer>().material = WestWallMaterial;
        if (floor != null)
            floor.GetComponent<Renderer>().material = floorMaterial;
       

    }
}
