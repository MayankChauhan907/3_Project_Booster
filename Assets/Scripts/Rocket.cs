using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //Referancess
    Rigidbody rigidBody;
    AudioSource audioSource;

    //Member Variables
    [SerializeField] float _RotationThrustSpeed = 100f;
    [SerializeField] float _mainThrustSpeed = 100f;
    [SerializeField] AudioClip _mainThrustSFX;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] AudioClip _winSFX;


    //Configuration
    enum State { Alive, Trascending, Dying }
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotationInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Trascending;
        audioSource.Stop();
        audioSource.PlayOneShot(_winSFX);
        Invoke("LoadNextScene", 1f);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(_deathSFX);
        Invoke("LoadFirstScene", 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * _mainThrustSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(_mainThrustSFX);
        }
    }

    private void RespondToRotationInput()
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
