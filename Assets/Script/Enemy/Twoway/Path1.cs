using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path1 : MonoBehaviour
{
   public Transform[] waypoints;
   public House house;              // อ้างอิงไปที่บ้านเป้าหมาย
   
       public Transform GetWaypoint(int index)
       {
           if (index >= 0 && index < waypoints.Length)
           {
               return waypoints[index];
           }
           return null;
       }
   
       public int WaypointCount
       {
           get { return waypoints.Length; }
       }
       
       // ฟังก์ชันนี้จะส่งคืนบ้านที่เป็นเป้าหมาย
       public House GetTargetHouse()
       {
           return house;
       }
}
