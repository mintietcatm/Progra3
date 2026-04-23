using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int scoreValue = 10; //Valor de los tickets. se me habia olvidado
    public AudioClip sonidoRecoger;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    public GameObject efecto;

    void Collect(GameObject player)
    {
        Instantiate(efecto, transform.position, Quaternion.identity);

        if (audioSource != null && sonidoRecoger != null)
        {
            audioSource.PlayOneShot(sonidoRecoger);
        }

        GameManager.instance.AddScore(scoreValue);
        GameManager.instance.AddTicket();

        GameObject fx = Instantiate(efecto, transform.position, Quaternion.identity);
        Destroy(fx, 2f);
    }

}