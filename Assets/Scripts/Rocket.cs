using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float _RotationThrustSpeed = 100f;
    [SerializeField] float _mainThrustSpeed = 100f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Movement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing
                break;

            case "Finish":
                SceneManager.LoadScene(1);
                break;

            default:
                //dead
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * _mainThrustSpeed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Movement()
    {
        rigidBody.freezeRotation = true;

        float RotateThisFrame = _RotationThrustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * RotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * RotateThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
