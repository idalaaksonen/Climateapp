using climatobservations.Models;
using climatobservations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace climatobservations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        DbRepository db;
        Category snowCategory; 

        public MainWindow()
        {
            InitializeComponent();

            db = new DbRepository();
            PopulateCategory();
        }


        // REVIDERAD 2022-04-09
        // ANVÄMD DATABASEN SOM SKICKADES IN I MOODLE DÅ DEN HAR CONSTRAINTS
        // ANVÄND DUBBELKLICK FÖR ATT SE OCH ÄNDRA OBSERVATIONER UNDER PRESENTERA OBSERVATIONER 
       

        private Observer GetObserver(string observerName)
        { 
            var subs = observerName.Split(' ');
            string firstName = subs[0];
            string lastName = subs[1];

            Observer observer = new Observer()
            {
                Firstname = firstName,
                Lastname = lastName
            };
            return observer;
        }

        private void btnAddObserver_Click(object sender, RoutedEventArgs e)
        {
            string observerName = txtNameObserver.Text;
            var observer = GetObserver(observerName);
            db.AddObserver(observer); // Skickar objektet till db
            MessageBox.Show($"Observatören är nu tillagd.");
            txtNameObserver.Text = null; 
        }

        private void btnRemoveObserver_Click(object sender, RoutedEventArgs e)
        {
            string observerName = txtNameObserver.Text;
            var observer = GetObserver(observerName);
   
            try
            {
                db.RemoveObserver(observer);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Allvarligt fel")
                {
                    throw;
                }
                else
                {
                    MessageBox.Show($"Du kan inte ta bort en observatör som har en observation.");
                }
            }
            txtNameObserver.Clear();
        }

        private void btnShowObserver_Click(object sender, RoutedEventArgs e)
        {
            var observers = db.GetObserver();
      
            lstbox.ItemsSource = observers.OrderBy(x => x.Lastname); //Sorterar efternamn https://stackoverflow.com/questions/188141/listt-orderby-alphabetical-order Linq-expression.
        }

        private void btnAddObservation_Click(object sender, RoutedEventArgs e) 
        {
            if (lstbox.SelectedItem == null || lstboxCategory.SelectedItem == null)
            {
                MessageBox.Show($"Lägg till observatör, kategori och mätpunkt.");
            }
            else
            {
                decimal value;
                var selectedObserver = (Observer)lstbox.SelectedItem; 
                var selectedCategory = (Category)lstboxCategory.SelectedItem;   

                try 
                {
                    value = Convert.ToDecimal(txtMeasurement.Text);
                }
                catch (Exception) // Catch fångar upp om value exempelvis innehåller bokstäver eller null, börjar sedan om via return
                {
                    MessageBox.Show($"Ange mätvärde i siffror.");
                    return; 
                }

                var observation = db.GetObservationByObserverId(selectedObserver); // Checkar om observation redan finns 

                if (observation == default(Observation)) // Om värdet är null så läggs ett datum för observation in
                {
                    db.AddObservation(selectedObserver, DateTime.Today); // Lägger automatiskt datum i observation - en observatör endast ha en observation dock flera mätvärden
                    observation = db.GetObservationByObserverId(selectedObserver);
                }

                if (selectedCategory.Id == 4) // Ändrad från sträng till objekt
                {
                    try
                    {
                        decimal snowdepth = Convert.ToDecimal(txtMeasurementSnow.Text);
                        db.AddMeasurement(observation, snowCategory, snowdepth);
                        ShowObservation(selectedObserver, observation, snowCategory, snowdepth);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Ange snödjup i cm.");
                        return;
                    }    
                }
                db.AddMeasurement(observation, selectedCategory, value);
                ShowObservation(selectedObserver, observation, selectedCategory, value);
                MessageBox.Show($"Tack! Din observation är nu tillagd.");
            }
        }

        private void lstbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lstbox.SelectedItem == null)
            {
                return;
            }

            txtMeasurement.Visibility = Visibility.Visible; 
            lbl.Visibility = Visibility.Visible;
            lstboxCategory.Visibility = Visibility.Visible;
            btnAddObservation.Visibility = Visibility.Visible;

            Observer observer = (Observer)lstbox.SelectedItem;   
            PopulateObservations(observer);
        }

        private void PopulateObservations(Observer observer)
        {
            var observation = db.GetObservationByObserverId(observer);

            if (observation == default(Observation))
            { 
                return; 
            }

            List <Measurement> measurements = db.GetMeasurementByObservationId(observation); // För att visa alla mätpunkter som är gjorda
            var categories = lstboxCategory.ItemsSource;
            //List <Category> categories = db.GetCategory();

            foreach (Measurement measurement in measurements)
            {
                foreach (Category category in categories)
                {
                    if (measurement.Category_Id == category.Id)
                    {
                        ShowObservation(observer, observation, category, (decimal)measurement.Value);
                    }
                }
            }
        }

        private void lstbox_MouseDoubleClick2(object sender, MouseButtonEventArgs e) // Ändra observation i listbox observation
        {
            var climateObservation = (Climate)(lstboxObservation.SelectedItem);

            lstbox.SelectedItem = climateObservation.observer;
            lstboxCategory.SelectedItem = climateObservation.category;
            txtMeasurement.Text = climateObservation.value.ToString(); 
        }

        private void PopulateCategory() // Populeras från start men är hidden tills observatör är tillagd
        {
            var categories = db.GetCategory();

            foreach (var category in categories)
            {
                if (category.Id == 11)
                {
                    snowCategory = category;
                }
            }

            lstboxCategory.ItemsSource = categories;
        }

   
        private void lstboxCategory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selectedCategory = (Category)lstboxCategory.SelectedItem;

            if (selectedCategory != default(Category) && selectedCategory.Id == 4)
            {
                txtMeasurementSnow.Visibility = Visibility.Visible;
            }
            else
            {
                txtMeasurementSnow.Visibility = Visibility.Hidden;
            }
        }

        private void ShowObservation(Observer observer, Observation observation, Category category, decimal value)
        {
            Climate climateObservation = new Climate();
            climateObservation.observation = observation;
            climateObservation.observer = observer;
            climateObservation.category = category;
            climateObservation.value = value;   
            lstboxObservation.Items.Add(climateObservation);
        }
    }
}

     