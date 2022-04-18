using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : PlayerController
{
  protected override void OnProcessInput()
  {
    input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    if (input.sqrMagnitude > 1f)
    {
      input = input.normalized;
    }
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      chargeSkill.StartCharging(Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.LeftShift))
    {
      chargeSkill.Update(Time.deltaTime);
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      Vector3 p = chargeSkill.Finish();
      transform.position = p;
    }
  }
}
