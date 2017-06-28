﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ProjectOxford.Text.Core
{
    /// <summary>
    /// Client for interacting with the Text Analytics API's. This is an abstract class.
    /// </summary>
    public abstract class TextClient
    {
        #region Fields

        private const string APPLICATION_JSON_CONTENT_TYPE = "application/json";

        private const string GET_METHOD = "GET";

        private const string OCP_APIM_SUBSCRIPTION_KEY = "Ocp-Apim-Subscription-Key";

        private const string POST_METHOD = "POST";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextClient"/> class.
        /// </summary>
        /// <param name="apiKey">The Text Analytics API key.</param>
        public TextClient(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the Text Analytics API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string ApiKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Text Analytics service URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sends the post to the Text Analytics API.
        /// </summary>
        /// <param name="data">The data to post to the Text Analytics API in json format.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected string SendPost(string data)
        {
            return SendPost(this.Url, data);
        }

        /// <summary>
        /// Sends the post to the Text Analytics API.
        /// </summary>
        /// <param name="url">The URL of the Text Analytics API.</param>
        /// <param name="data">The data to post to the Text Analytics API in json format.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected string SendPost(string url, string data)
        {
            return this.SendPostAsync(url, data).Result;
        }

        /// <summary>
        /// Sends the post to the Text Analytics API asynchronously.
        /// </summary>
        /// <param name="data">The data to post to the Text Analytics API in json format.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected async Task<string> SendPostAsync(string data)
        {
            return await SendPostAsync(this.Url, data);
        }

        /// <summary>
        /// Sends the post to the Text Analytics API asynchronously.
        /// </summary>
        /// <param name="url">The URL of the Text Analytics API.</param>
        /// <param name="data">The data to post to the Text Analytics API in json format.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when either the URL or API key is provided.
        /// </exception>
        protected async Task<string> SendPostAsync(string url, string data)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            if (String.IsNullOrWhiteSpace(this.ApiKey))
            {
                throw new ArgumentException(nameof(ApiKey));
            }

            if (String.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException(nameof(data));
            }

            byte[] reqData = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers[OCP_APIM_SUBSCRIPTION_KEY] = this.ApiKey;
            request.ContentType = APPLICATION_JSON_CONTENT_TYPE;
            request.Accept = APPLICATION_JSON_CONTENT_TYPE;

            //request.ContentLength = reqData.Length;
            request.Method = POST_METHOD;

            using (var reqStream = await request.GetRequestStreamAsync())
            {
                reqStream.Write(reqData, 0, reqData.Length);
            }

            var responseData = "";

            using (var response = await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseData = reader.ReadToEnd();
                    }
                }
            }

            return responseData;
        }

        /// <summary>
        /// Sends the get to the Text Analytics API.
        /// </summary>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected string SendGet()
        {
            return SendGet(this.Url);
        }

        /// <summary>
        /// Sends the get to the Text Analytics API.
        /// </summary>
        /// <param name="url">The URL of the Text Analytics API.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected string SendGet(string url)
        {
            return SendGetAsync(url).Result;
        }

        /// <summary>
        /// Sends the get to the Text Analytics API asynchronously.
        /// </summary>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        protected async Task<string> SendGetAsync()
        {
            return await SendGetAsync(this.Url);
        }

        /// <summary>
        /// Sends the get to the Text Analytics API asynchronously.
        /// </summary>
        /// <param name="url">The URL of the Text Analytics API.</param>
        /// <returns>Returns a JSON string from the Text Analytics API.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when either the URL or API key is provided.
        /// </exception>
        protected async Task<string> SendGetAsync(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            if (String.IsNullOrWhiteSpace(this.ApiKey))
            {
                throw new ArgumentException(nameof(ApiKey));
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers[OCP_APIM_SUBSCRIPTION_KEY] = this.ApiKey;
            request.Method = GET_METHOD;

            var responseData = "";

            using (var response = await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseData = reader.ReadToEnd();
                    }
                }
            }

            return responseData;
        }

        #endregion Methods
    }
}
