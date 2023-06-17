using DNS_System.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DNS_System.DALL
{
    public class DNSDataService
    {
        #region Fields

        private string FilePath;

        #endregion

        #region CTORs

        public DNSDataService()
        {
            FilePath = Path.Combine(Application.persistentDataPath, "CustomDnsData");
        }

        #endregion

        #region Public Methods

        public void Save(List<DNSData> dnsData)
        {
            try
            {
                CheckOrCreateFile();

                using (var writer = new StreamWriter(FilePath))
                {
                    var content = Newtonsoft.Json.JsonConvert.SerializeObject(dnsData);

                    writer.Write(content);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public List<DNSData> Load()
        {
            try
            {
                CheckOrCreateFile();

                using (var reader = new StreamReader(FilePath))
                {
                    string rawData = null;

                    rawData = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(rawData))
                        return new List<DNSData>();

                    var dnsData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DNSData>>(rawData);

                    if (dnsData == null)
                        dnsData = new List<DNSData>();

                    return dnsData;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);

                return new List<DNSData>();
            }
        }

        #endregion

        #region Private Methods

        private void CheckOrCreateFile()
        {
            if (File.Exists(FilePath))
                return;

            using (File.Create(FilePath)) { }
        }

        #endregion
    }
}