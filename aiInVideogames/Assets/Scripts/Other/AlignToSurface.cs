using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToSurface : MonoBehaviour
{
  void OnDrawGizmos()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, -transform.up, out hit))
    {
      Gizmos.color = Color.red;
      Gizmos.DrawLine(transform.position, hit.point);
      Gizmos.DrawSphere(hit.point, 0.1f);
      // Reflect the line
      Vector3 reflected = Vector3.Reflect(transform.forward, hit.normal);
    }
  }

  void Update()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, -transform.up, out hit))
    {
      Vector3 groundNormal = hit.normal;
      Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal);
      transform.rotation = targetRotation * transform.rotation;
    }
  }

}
