using UnityEngine;

public class SonidosMenu : MonoBehaviour
{
    [SerializeField] private AudioSource fuenteSFX;// referencia al componente AudioSource para reproducir efectos de sonido
    [SerializeField] private AudioClip[] efectos;// vector que contiene los efectos de sonido disponibles

    public void ReproducirSonido()// método para reproducir un efecto de sonido aleatorio
    {
        if (fuenteSFX != null && efectos.Length > 0)// verifica que el componente AudioSource y el vector de efectos no estén vacíos
        {
            int indice = Random.Range(0, efectos.Length);// genera un índice aleatorio dentro del rango del vector de efectos
            fuenteSFX.PlayOneShot(efectos[indice]);// reproduce el efecto de sonido correspondiente al índice generado
        }
    }
}