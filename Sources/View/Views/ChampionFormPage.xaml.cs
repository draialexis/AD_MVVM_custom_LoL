using System.Diagnostics;
using View.AppVM;
using ViewModel;

namespace View.Views
{
    public partial class ChampionFormPage : ContentPage
    {
        public ChampionFormVM ChampionFormVM => championFormVM;
        private readonly ChampionFormVM championFormVM;

        public ChampionsMgrVM ChampionsMgrVM => championsMgrVM;
        private readonly ChampionsMgrVM championsMgrVM;

        public MainAppVM MainAppVM => mainAppVM;
        private readonly MainAppVM mainAppVM;

        public ChampionFormPage(ChampionFormVM championFormVM, ChampionsMgrVM championsMgrVM, MainAppVM mainAppVM)
        {
            this.championFormVM = championFormVM;
            this.championsMgrVM = championsMgrVM;
            this.mainAppVM = mainAppVM;
            InitializeComponent();
            BindingContext = ChampionFormVM;
        }

        private void OnAddCharacteristicClicked(object sender, EventArgs e)
        {
            NewCharacteristicKey.Text = "";
            NewCharacteristicValue.Text = "";
        }

        public async void OnUploadIconButtonClicked(object sender, EventArgs e)
        {
            var imageBytes = await UploadImage();
            if (imageBytes != null)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    ChampionFormVM.UpsertIconCommand.Execute(imageBytes);
                    return Task.CompletedTask;
                });
            }
        }

        public async void OnUploadImageButtonClicked(object sender, EventArgs e)
        {
            var imageBytes = await UploadImage();
            if (imageBytes != null)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    ChampionFormVM.UpsertImageCommand.Execute(imageBytes);
                    return Task.CompletedTask;
                });
            }
        }

        public async static Task<byte[]?> UploadImage()
        {
            try
            {
                var pickedFile = await FilePicker.Default.PickAsync(PickOptions.Images);

                if (pickedFile != null)
                {
                    using var stream = await pickedFile.OpenReadAsync();
                    byte[] bytes = new byte[stream.Length];
                    await stream.ReadAsync(new Memory<byte>(bytes));
                    return bytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
