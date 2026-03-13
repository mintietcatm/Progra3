using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private List<string> buffs;
    ObjectSpawn objectPooling;

    [System.Obsolete]
    private void Awake()
    {
        objectPooling = FindObjectOfType<ObjectSpawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "TicketBase":
                {
                    objectPooling.ReturnToList(this.transform);
                    break;
                }
            case "TicketMediano":
                {
                    objectPooling.ReturnToList(this.transform);
                    break;
                }
            case "TicketGrande":
                {
                    objectPooling.ReturnToList(this.transform);
                    break;
                }
            case var buff when buffs.Contains(other.tag):
                {
                    break;
                }
        }
    }

    private void OnTriggerExit (Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
