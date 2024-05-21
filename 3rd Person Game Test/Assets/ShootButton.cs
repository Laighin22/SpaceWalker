using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public ShipGuns gun1;
    public ShipGuns gun2;

    public void OnPointerDown(PointerEventData eventData)
    {
        gun1.SetShooting(true);
        gun2.SetShooting(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gun1.SetShooting(false);
        gun2.SetShooting(false);
    }
}
