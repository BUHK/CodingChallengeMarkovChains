using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace TravelingSalesPerson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            Trace.WriteLine("helloooooooooooooooooooooooo world!");
        }

        /// <summary>
        /// Setup the route and create UI elements
        /// </summary>
        void InitialDraw()
        {
            cityCount = 20;
            List<City> cities = new List<City>();

            for (int i = 0; i < cityCount; i++)
            {
                City city = new City(i, 4);
                // generate location based on a seed
                city.SetRandomLocation(canvas, i);

                // generate location without seed
                //city.SetRandomLocation(canvas);

                Ellipse ellipse = new Ellipse
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = city.radius * 2,
                    Height = city.radius * 2
                };

                Canvas.SetLeft(ellipse, city.Point.X - city.radius);
                Canvas.SetTop(ellipse, city.Point.Y - city.radius);
                canvas.Children.Add(ellipse);
                cities.Add(city);
            }

            workingRoute = new Route(cities);
            canvas.Children.Add(workingRoute.polyline);

            randomRoute = new Route(cities);
            lexicoGraphicRoute = new Route(cities);
            population = new Population(new Route(cities), 200, 4);
            // The number of starting routes for subsequent generations in genetic algorithm
            startingRouteCount = 4;

            //workingRoute.ShuffleCities();
            crossoverPop = new Population(new Route(cities), 200, cityCount);
            crossoverPop.GeneratePopulation();

            SetRecordDistance();
            SetIterationCount();
        }

        /// <summary>
        /// 
        /// </summary>
        void Draw()
        {
            switch (drawSwitch)
            {
                case "random":
                    if (!pause)
                    {
                        //do as many swaps as there are cities * 2
                        workingRoute.ShuffleCities();
                        SetIterationCount();

                        if (recordDistance > workingRoute.Distance)
                        {
                            CopyToCanvas2();
                            SetRecordDistance();
                            //pause = !pause;
                        }
                    }
                    break;

                case "lexicographic":
                    if (!pause && !workingRoute.lastOrder)
                    {
                        workingRoute.NextLexicographicOrder();
                        if (!workingRoute.lastOrder)
                        {
                            SetIterationCount();
                        }

                        if (recordDistance > workingRoute.Distance)
                        {
                            CopyToCanvas2();
                            SetRecordDistance();
                            //pause = !pause;
                        }
                    }
                    // if not paused and last order reached, then pause
                    else
                    {
                        pause = true;
                    }
                    break;

                case "genetic":
                    if (!pause)
                    {
                        //population = new Population(workingRoute, 10);
                        population.GeneratePopulation();

                        // Show each route of the population
                        // This doesn't seem to work, need to exit draw to update UI
                        //foreach (var route in population.Routes)
                        //{
                        //    ChangeWorkingRoute(route);
                        //}

                        ChangeWorkingRoute(population.GetShortestRoute());

                        population = population.NextGeneration(startingRouteCount);

                        //Trace.WriteLine(population.ToString());
                        //Trace.WriteLine("genetic loop");

                        // pause after each iteration
                        //pause = true;

                        SetIterationCount();

                        if (recordDistance > workingRoute.Distance)
                        {
                            CopyToCanvas2();
                            SetRecordDistance();
                        }
                    }
                    break;

                case "crossover":
                    if (!pause)
                    {
                        ChangeWorkingRoute(crossoverPop.GetShortestRoute());

                        crossoverPop = crossoverPop.NextCrossoverGen();

                        //Trace.WriteLine(crossoverPop.ToString());
                        //Trace.WriteLine("crossover loop");

                        // pause after each iteration
                        //pause = true;

                        SetIterationCount();

                        if (recordDistance > workingRoute.Distance)
                        {
                            CopyToCanvas2();
                            SetRecordDistance();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void ChangeWorkingRoute(Route newRoute)
        {
            canvas.Children.Remove(workingRoute.polyline);
            workingRoute = newRoute;
            canvas.Children.Add(workingRoute.polyline);
        }

        private void SetIterationCount()
        {
            iterationCount++;
            this.IterationCount.Text = $"Generation: {iterationCount}";
        }

        private void SetRecordDistance()
        {
            recordDistance = workingRoute.Distance;
            this.RecordDistance.Text = $"Shortest distance: {recordDistance:N1}";
        }

        /// <summary>
        /// Copy to second canvas(canvas2)
        /// </summary>
        private void CopyToCanvas2()
        {
            canvas2.Children.Clear();
            foreach (UIElement child in canvas.Children)
            {
                var xaml = XamlWriter.Save(child);
                var deepCopy = XamlReader.Parse(xaml) as UIElement;
                canvas2.Children.Add(deepCopy);
            }
        }

        #region events
        /// <summary>
        /// Starts the tick interval that controls the main loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitialDraw();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Tick event that calls what should happen on every tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            Draw();
        }

        /// <summary>
        /// Button click event that starts/stops the random city shuffling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomRouteButton_Click(object sender, RoutedEventArgs e)
        {
            drawSwitch = "random";
            ChangeWorkingRoute(randomRoute);
            Trace.WriteLine("random select");
            pause = !pause;
        }

        /// <summary>
        /// Button click event that starts/stops the lexicographic order sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LexicographicButton_Click(object sender, RoutedEventArgs e)
        {
            drawSwitch = "lexicographic";
            ChangeWorkingRoute(lexicoGraphicRoute);
            Trace.WriteLine("lexicographic select");
            pause = !pause;
        }

        /// <summary>
        /// Button click event that starts/stops the genetic order sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneticSelectButton_Click(object sender, RoutedEventArgs e)
        {
            drawSwitch = "genetic";
            //ChangeWorkingRoute(geneticRoute);
            Trace.WriteLine("genetic select");
            pause = !pause;
        }

        /// <summary>
        /// Button click event that start the gentic crossover algorithm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneticCrossoverButton_Click(object sender, RoutedEventArgs e)
        {
            drawSwitch = "crossover";
            Trace.WriteLine("crossover select");
            pause = !pause;
        }
        #endregion

        #region test methods
        /// <summary>
        /// 
        /// </summary>
        void LexicographicTest()
        {
            List<int> cities = new List<int>
            {
                1,
                2,
                3
            };
            while (Route.LexicographicSortInt(cities)) ;
        }

        void CrossoverTest()
        {
            List<int> a = new List<int> { 1, 2, 3, 4, 5 };
            List<int> b = new List<int> { 6, 4, 3, 2, 1 };

            a = Population.Crossover(a, b);
            string str = "";
            foreach (var item in a)
            {
                str += $"{ item},";
            }

            Trace.WriteLine(str);
        }

        #endregion

    }
}
