using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
       
        
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private bool lookAtDogo;
        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
            
            
        }

        private void Update()
        {
            if (target == null) return;
           

            if (InventoryManager.instance.GetInventoryItem("Key of Slime") &&
                InventoryManager.instance.GetInventoryItem("Key of Nature") &&
                InventoryManager.instance.GetInventoryItem("Key of Stone"))
            {
                 DogoTotemScripts Dogo = FindObjectOfType<DogoTotemScripts>();
                if (!lookAtDogo && Dogo != null)
                {
                    target = Dogo.gameObject.transform;
                    DogoCamera();
                }
                

            }

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);

           
        }
        
         void DogoCamera()
        {
            Invoke("ResetCamera", 3f);
            lookAtDogo = true;
        }
        void ResetCamera()
        {
            Movement player = FindObjectOfType<Movement>();
            target = player.gameObject.transform;
            
        }

    }
}
