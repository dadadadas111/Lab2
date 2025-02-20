using UnityEngine;
using UnityEngine.UI;

public class BuffLoader : MonoBehaviour
{
    public Image buffFillImage;
    private float buffDuration;
    private float buffTimeRemaining;
    private bool isBuffActive = false;

    public void StartBuff(float duration)
    {
        buffDuration = duration;
        buffTimeRemaining = duration;
        isBuffActive = true;
        buffFillImage.fillAmount = 1f;
        buffFillImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isBuffActive)
        {
            buffTimeRemaining -= Time.deltaTime;
            buffFillImage.fillAmount = buffTimeRemaining / buffDuration;

            if (buffTimeRemaining <= 0f)
            {
                EndBuff();
            }
        }
    }

    private void EndBuff()
    {
        isBuffActive = false;
        buffFillImage.gameObject.SetActive(false);
    }
}
