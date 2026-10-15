using UnityEngine;
using UnityEngine.UI;

public class VolumenMusica : MonoBehaviour
{
    // referencia al componente AudioSource que reproduce la música
    [SerializeField] private AudioSource musica;
    // referencia al componente Slider que controla el volumen de la música
    [SerializeField] private Slider sliderMusica;
    // variable que almacena el valor del volumen de la música
    private float volumenAplicado;
    private float volumenTemporal;
    public bool HayCambios => !Mathf.Approximately(volumenTemporal, volumenAplicado);

    private void Start()
    {
        // obtiene el valor del volumen de la música guardado en PlayerPrefs, o 0.5 si no existe
        volumenAplicado = Mathf.Clamp01(PlayerPrefs.GetFloat("VolumenMusica", 0.5f));
        Descartar();
        // establece el volumen del AudioSource al valor guardado
        musica.volume = volumenAplicado;
        // agrega un listener al evento onValueChanged del slider para llamar al método CambiarVolumen cuando el valor cambie
        sliderMusica.onValueChanged.AddListener(CambiarVolumen);
    }

    // método que se llama cuando el valor del slider cambia
    private void CambiarVolumen(float valor)
    {
        // Previsualiza el volumen sin modificar el valor aplicado ni PlayerPrefs.
        // actualiza la variable volumenTemporal con el nuevo valor del slider
        volumenTemporal = valor;
        musica.volume = volumenTemporal;
    }

    public void Aplicar()
    {
        volumenAplicado = volumenTemporal;
        // actualiza el volumen del AudioSource al nuevo valor
        musica.volume = volumenAplicado;
        // guarda el nuevo valor del volumen de la música en PlayerPrefs
        PlayerPrefs.SetFloat("VolumenMusica", volumenAplicado);
    }

    public void Descartar()
    {
        volumenTemporal = volumenAplicado;
        // establece el valor del slider sin invocar el evento
        sliderMusica.SetValueWithoutNotify(volumenTemporal);
        // Al descartar, el sonido vuelve al ultimo volumen aplicado.
        musica.volume = volumenAplicado;
    }

    public void ValoresPredeterminados()
    {
        volumenTemporal = 0.5f;
        sliderMusica.SetValueWithoutNotify(volumenTemporal);
        musica.volume = volumenTemporal;
    }

    private void OnDestroy()
    {
        if (sliderMusica != null)
        {
            sliderMusica.onValueChanged.RemoveListener(CambiarVolumen);
        }
    }
}
