using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRegler : MonoBehaviour
{
    private bool isTriggered;
    private bool turnRegler;

   public void TurneRegler()
    {
        if (isTriggered == false)
        {
            turnRegler = true;
            isTriggered = true; 
        }
    }

    private void Update()
    {

       if(turnRegler == true)
        {
            transform.Rotate(0, 0, Time.deltaTime * 20);
           
            
            if(transform.eulerAngles.z == 270)
            {
                Debug.Log("jetzt");
                turnRegler = false; 
            }
        }
    }

}
