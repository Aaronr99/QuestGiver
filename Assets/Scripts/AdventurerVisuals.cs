using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AdventurerVisuals : MonoBehaviour
{
    public Adventurer relationedAdventurer;
    public Animator animator;
    public GameObject handGO;

    public void MoveCharacter()
    {
        StartCoroutine(MoveChar());
    }

    private IEnumerator MoveChar()
    {
        GameObject adventurerGO = gameObject;
        float speedMod = 1;
        if (Mathf.Abs(adventurerGO.transform.position.x) > 3f)
        {
            speedMod = 0.75f;
        }
        if (Vector3.Distance(adventurerGO.transform.position, Vector3.zero) < 2.5f)
        {
            animator.CrossFade("Stand", 0.1f, 0);
            yield return new WaitForSeconds(0.45f);
        }
        Vector3 targetPosition = adventurerGO.transform.position + Vector3.right * 4f;
        animator.CrossFade("Walk", 0.1f, 0);
        Quaternion originalRotation = adventurerGO.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f);
        while (Quaternion.Angle(adventurerGO.transform.rotation, targetRotation) > 0.5f)
        {
            adventurerGO.transform.rotation = Quaternion.RotateTowards(adventurerGO.transform.rotation, targetRotation, 250f * Time.deltaTime * speedMod);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(adventurerGO.transform.position, targetPosition) > 0.05f)
        {
            adventurerGO.transform.position = Vector3.MoveTowards(adventurerGO.transform.position, targetPosition, Time.deltaTime * 7f * speedMod);
            yield return new WaitForEndOfFrame();
        }
        while (Quaternion.Angle(adventurerGO.transform.rotation, originalRotation) > 0.5f)
        {
            adventurerGO.transform.rotation = Quaternion.RotateTowards(adventurerGO.transform.rotation, originalRotation, 250f * Time.deltaTime * speedMod);
            yield return new WaitForEndOfFrame();
        }
        if (Vector3.Distance(adventurerGO.transform.position, Vector3.zero) < 2.5f)
        {
            animator.CrossFade("Sit", 0.3f, 0);
        }
        else
        {
            animator.CrossFade("Iddle", 0.1f, 0);
        }
        yield return new WaitForEndOfFrame();
    }
}
