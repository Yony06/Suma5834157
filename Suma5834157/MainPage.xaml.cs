namespace Suma5834157
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResultadoId;
        int count = 0;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => Listview.ItemsSource = await _dbService.GetResultado());
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

        }

        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var resultado = (Resultado)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResultadoId = resultado.Id;
                    Entryprimernumero.Text = resultado.Numero1;
                    Entrysegundonumero.Text = resultado.Numero2;
                    labelresultado.Text = resultado.Suma;
                    break;

                case "Delete":
                    await _dbService.Delete(resultado);
                    Listview.ItemsSource = await _dbService.GetResultado();
                    break;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void sumarBtn_Clicked(object sender, EventArgs e)
        {
            if (double.TryParse(Entryprimernumero.Text, out double numero1) && double.TryParse(Entrysegundonumero.Text, out double numero2))
            {
                // Realizar la suma
                double suma = numero1 + numero2;

                // Asignar el resultado al label
                labelresultado.Text = suma.ToString();

                if (_editResultadoId == 0)
                {
                    // Crear nuevo resultado
                    await _dbService.Create(new Resultado
                    {
                        Numero1 = Entryprimernumero.Text,
                        Numero2 = Entrysegundonumero.Text,
                        Suma = labelresultado.Text
                    });
                }
                else
                {
                    // Actualizar resultado existente
                    await _dbService.Update(new Resultado
                    {
                        Id = _editResultadoId,
                        Numero1 = Entryprimernumero.Text,
                        Numero2 = Entrysegundonumero.Text,
                        Suma = labelresultado.Text
                    });
                    _editResultadoId = 0;
                }

                // Limpiar entradas y resultado
                Entryprimernumero.Text = string.Empty;
                Entrysegundonumero.Text = string.Empty;
                labelresultado.Text = string.Empty;

                // Actualizar la lista
                Listview.ItemsSource = await _dbService.GetResultado();
            }
            else
            {
                // Manejar el caso en que la conversión falla (entradas no válidas)
                await DisplayAlert("Error", "Por favor ingrese números válidos.", "OK");
            }
        }
    }
}
