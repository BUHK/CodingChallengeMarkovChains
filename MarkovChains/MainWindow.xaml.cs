using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using MarkovChains;

namespace TravelingSalesPerson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBoxOutputter outputter;

        public MainWindow()
        {
            InitializeComponent();
            Trace.WriteLine("helloooooooooooooooooooooooo world!");

            outputter = new TextBoxOutputter(OutConsole);
            Console.SetOut(outputter);
            Console.WriteLine("ello world");
        }

        /// <summary>
        /// 
        /// </summary>
        void InitialDraw()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        void Draw()
        {
            
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

        #endregion
    }
}
