using UnityEngine;
using UnityEngine.UI;

public class PantallaCompletaMenu : MonoBehaviour
{
    //referencia al componente Toggle que permite activar o desactivar el modo de pantalla completa
    [SerializeField] private Toggle togglePantallaCompleta;
    private bool modoAplicado;
    private bool modoTemporal;
    public bool HayCambios => modoTemporal != modoAplicado;
    public bool PantallaCompleta => modoTemporal;

    private void Start()
    {
        //inicializamos el valor inicial en 0 (modo ventana)
        int inicial = 0;
        //verificamos si la pantalla está en modo completo
        if (Screen.fullScreen) { inicial = 1; }
        //obtenemos el valor guardado en PlayerPrefs, si no existe se asigna el valor inicial
        modoAplicado = PlayerPrefs.GetInt("PantallaCompleta", inicial) == 1;
        Descartar();
        togglePantallaCompleta.onValueChanged.AddListener(CambiarModo);
        // ResolucionMenu restaura resolucion y modo juntos al iniciar.
    }

    private void CambiarModo(bool valor)
    {
        modoTemporal = valor;
        // Prueba el modo junto a la resolucion temporal, sin guardar.
        GetComponent<ResolucionMenu>().Previsualizar(modoTemporal);
    }

    public void Aplicar()
    {
        modoAplicado = modoTemporal;
        //inicializamos el valor guardado en 0 (modo ventana)
        int valor = 0;
        if (modoAplicado) { valor = 1; }
        //guardamos el valor en PlayerPrefs para que se mantenga entre sesiones
        PlayerPrefs.SetInt("PantallaCompleta", valor);
    }

    public void Descartar()
    {
        modoTemporal = modoAplicado;
        togglePantallaCompleta.SetIsOnWithoutNotify(modoTemporal);
    }

    public void ValoresPredeterminados()
    {
        modoTemporal = false;
        togglePantallaCompleta.SetIsOnWithoutNotify(false);
    }

    private void OnDestroy()
    {
        if (togglePantallaCompleta != null)
        { togglePantallaCompleta.onValueChanged.RemoveListener(CambiarModo); }
    }
}
