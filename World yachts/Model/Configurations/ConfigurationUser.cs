using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace World_yachts.Model.Configurations
{
    [Serializable]
    public class ConfigurationUser
    {
        public int IdUser { get; set; }

        public string TypeUser { get; set; }

        public bool IsAuthorized { get; set; }

        public ConfigurationUser(int idUser, string typeUser)
        {
            IdUser = idUser;
            TypeUser = typeUser;
            IsAuthorized = false;
        }

        public void Save()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream("ConfigurationUser.bin", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, this);
            }
        }

        public static ConfigurationUser GetConfiguration()
        {
            if (File.Exists("ConfigurationUser.bin"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream("ConfigurationUser.bin", FileMode.Open))
                {
                    return binaryFormatter.Deserialize(fileStream) as ConfigurationUser;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
