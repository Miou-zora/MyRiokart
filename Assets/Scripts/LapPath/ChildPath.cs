using UnityEngine;

public class ChildPath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<LapPath>().OnTriggerEnterChildPath(other, this);
    }
}
