using UnityEngine;

public class ProgressFill : MonoBehaviour
{
    public float _FillRateValue = -0.51f; //progress bar starts empty
    Material objectMaterial;

    float stepSize = 0.1f; //progress is done by this value

    // Start is called before the first frame update
    void Start()
    {
        objectMaterial = new Material(Shader.Find("Shader Graphs/ProgressBarSingleAxis")); //creating a material with the shader
        gameObject.GetComponent<Renderer>().material = objectMaterial; //new material is applied to the game object
        objectMaterial.SetFloat("_FillRate", _FillRateValue); //initial value is set 
    }


    public void ChangeValue(bool increase) //enables changing the value of progress bar
    {                                   //if increase param is true, the progress bar progresses otherwise it deprogresses
        if (increase)
        {
            _FillRateValue += stepSize; //progress increased
        }
        else
        {
            _FillRateValue -= stepSize; //progress decreased
        }
        objectMaterial.SetFloat("_FillRate", _FillRateValue); //Update the value of the progress bar
    }
}