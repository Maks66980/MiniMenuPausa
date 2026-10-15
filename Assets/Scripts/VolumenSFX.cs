using UnityEngine;
using UnityEngine.UI;

public class VolumenSFX : MonoBehaviour
{
    //refencia al componente AudioSource que reproduce los efectos de sonido
    [SerializeField] private AudioSource fuenteSFX;
    //refencia al componente Slider que controla el volumen de los efectos de sonido
    [SerializeField] private Slider sliderSFX;
    //variable para almacenar el valor del volumen de los efectos de sonido
    private float volumenAplicado;
    private float volumenTemporal;
    public bool HayCambios => !Mathf.Approximately(volumenTemporal, volumenAplicado);

    private void Start()
    {
        //obtenemos el valor del volumen de los efectos de sonido guardado en PlayerPrefs, si no existe se asigna un valor por defecto de 0.5f
        volumenAplicado = Mathf.Clamp01(PlayerPrefs.GetFloat("VolumenSFX", 0.5f));
        Descartar();
        fuenteSFX.volume = volumenAplicado;
        sliderSFX.onValueChanged.AddListener(CambiarVolumen);
    }

    private void CambiarVolumen(float valor)
    {
        // Previsualiza el volumen sin modificar el valor aplicado ni PlayerPrefs.
        volumenTemporal = valor;
        fuenteSFX.volume = volumenTemporal;
    }

    public void Aplicar()
    {
        volumenAplicado = volumenTemporal;
        fuenteSFX.volume = volumenAplicado;
        PlayerPrefs.SetFloat("VolumenSFX", volumenAplicado);
    }

    public void Descartar()
    {
        volumenTemporal = volumenAplicado;
        //asignamos el valor del volumen de los efectos de sonido al slider sin notificar el cambio para evitar que se llame al evento 
        sliderSFX.SetValueWithoutNotify(volumenTemporal);
        // Al descartar, el sonido vuelve al ultimo volumen aplicado.
        fuenteSFX.volume = volumenAplicado;
    }

    public void ValoresPredeterminados()
    {
        volumenTemporal = 0.5f;
        sliderSFX.SetValueWithoutNotify(volumenTemporal);
        fuenteSFX.volume = volumenTemporal;
    }

    private void OnDestroy()
    {
        if (sliderSFX != null)
        {
            sliderSFX.onValueChanged.RemoveListener(CambiarVolumen);
        }
    }
}
