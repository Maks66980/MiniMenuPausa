using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolucionMenu : MonoBehaviour
{
    //referencia al componente TMP_Dropdown que muestra las opciones de resolución
    [SerializeField] private TMP_Dropdown dropdownResolucion;
    //lista para almacenar las resoluciones disponibles
    private readonly List<Vector2Int> resoluciones = new List<Vector2Int>();
    private int indiceAplicado;
    private int indiceTemporal;
    private int indicePredeterminado;
    public bool HayCambios => indiceTemporal != indiceAplicado;

    private void Start()
    {
        //iteramos sobre todas las resoluciones disponibles en el monitor de usuario
        foreach (Resolution disponible in Screen.resolutions)
        {
            //creamos un Vector2Int con el ancho y alto de la resolución
            Vector2Int tamano = new Vector2Int(disponible.width, disponible.height);
            //verificamos si la resolución ya está en la lista para evitar duplicados
            if (!resoluciones.Contains(tamano)) { resoluciones.Add(tamano); }
        }
        // Incluye el tamaño actual si no aparece en la lista.
        Vector2Int actual = new Vector2Int(Screen.width, Screen.height);
        //verificamos si la resolución actual ya está en la lista
        if (!resoluciones.Contains(actual)) { resoluciones.Add(actual); }
        indicePredeterminado = resoluciones.IndexOf(new Vector2Int(
            Screen.currentResolution.width, Screen.currentResolution.height));
        if (indicePredeterminado < 0) { indicePredeterminado = resoluciones.IndexOf(actual); }

        Vector2Int guardada = new Vector2Int(
            //obtenemos el ancho guardado en PlayerPrefs, si no existe se asigna el ancho actual de la pantalla
            PlayerPrefs.GetInt("ResolucionAncho", actual.x),
            //obtenemos el alto guardado en PlayerPrefs, si no existe se asigna el alto actual de la pantalla
            PlayerPrefs.GetInt("ResolucionAlto", actual.y));
        indiceAplicado = resoluciones.IndexOf(guardada);
        if (indiceAplicado < 0) { indiceAplicado = resoluciones.IndexOf(actual); }
        //lista para almacenar las opciones de resolución en formato de texto
        List<string> opciones = new List<string>();
        //iteramos sobre todas las resoluciones disponibles
        foreach (Vector2Int tamano in resoluciones)
        //agregamos la resolución a la lista de opciones en formato de texto
        { opciones.Add(tamano.x + " x " + tamano.y); }
        //limpiamos las opciones actuales del dropdown
        dropdownResolucion.ClearOptions();
        //agregamos las nuevas opciones al dropdown
        dropdownResolucion.AddOptions(opciones);
        Descartar();
        int inicial = 0;
        if (Screen.fullScreen) { inicial = 1; }
        //aplicamos la resolución guardada o la actual si no hay ninguna guardada
        AplicarAlJuego(PlayerPrefs.GetInt("PantallaCompleta", inicial) == 1);
        dropdownResolucion.onValueChanged.AddListener(CambiarResolucion);
    }

    private void CambiarResolucion(int indice) { indiceTemporal = indice; }

    public void Aplicar(bool pantallaCompleta)
    {
        indiceAplicado = indiceTemporal;
        AplicarAlJuego(pantallaCompleta);
        //obtenemos la resolución seleccionada de la lista
        Vector2Int tamano = resoluciones[indiceAplicado];
        //guardamos el ancho de la resolución seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("ResolucionAncho", tamano.x);
        //guardamos el alto de la resolución seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("ResolucionAlto", tamano.y);
    }

    private void AplicarAlJuego(bool pantallaCompleta)
    {
        FullScreenMode modo = FullScreenMode.Windowed;
        if (pantallaCompleta) { modo = FullScreenMode.FullScreenWindow; }
        Vector2Int tamano = resoluciones[indiceAplicado];
        Screen.SetResolution(tamano.x, tamano.y, modo);
    }

    public void Descartar()
    {
        indiceTemporal = indiceAplicado;
        //establecemos el valor seleccionado sin notificar a los listeners
        dropdownResolucion.SetValueWithoutNotify(indiceTemporal);
        //actualizamos la visualización del dropdown
        dropdownResolucion.RefreshShownValue();
    }

    public void ValoresPredeterminados()
    {
        indiceTemporal = indicePredeterminado;
        dropdownResolucion.SetValueWithoutNotify(indiceTemporal);
        dropdownResolucion.RefreshShownValue();
    }

    private void OnDestroy()
    {
        if (dropdownResolucion != null)
        { dropdownResolucion.onValueChanged.RemoveListener(CambiarResolucion); }
    }
}
