using Microsoft.Web.WebView2.Core;
using System.Windows;

namespace InteractiveMapWWM
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.NavigationCompleted += WebView_NavigationCompleted;
        }

        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;

            string extractionScript = @"
                (function() {
                    if (document.getElementById('map-extractor-style')) return;

                    var style = document.createElement('style');
                    style.id = 'map-extractor-style';
                    style.innerHTML = `
                        body, html { overflow: hidden !important; margin: 0 !important; padding: 0 !important; background: #1a1a1a !important; }

                        .leaflet-container, #map, .map-container {
                            position: fixed !important;
                            top: 0 !important;
                            left: 0 !important;
                            width: 100vw !important;
                            height: 100vh !important;
                            z-index: 2147483647 !important; 
                            box-sizing: border-box !important;
                        }

                        header, footer, nav, aside, .sidebar, .menu, #__next > div > div:not([class*='map']), .buttons.is-flex.is-justify-content-center {
                            display: none !important;
                        }
                        .adsbygoogle, [id*='google_ads'], [class*='ad-container'], [class*='ad_wrapper'], [id*='div-gpt-ad'], div[id*='sticky-ad'], div[class*='sticky-ad'], div[style*='position: fixed'][style*='bottom'], iframe[src*='google'], iframe[src*='ads'], iframe[id*='google'], .google-auto-placed {
                            display: none !important;
                            visibility: hidden !important;
                            opacity: 0 !important;
                            width: 0 !important;
                            height: 0 !important;
                            min-height: 0 !important;
                            max-height: 0 !important;
                            padding: 0 !important;
                            margin: 0 !important;
                            pointer-events: none !important;
                            z-index: -9999 !important;
                            position: absolute !important;
                        }
                    `;
                    document.head.appendChild(style);
                })();
            ";
            await System.Threading.Tasks.Task.Delay(500);
            await webView.CoreWebView2.ExecuteScriptAsync(extractionScript);
        }



    }
}