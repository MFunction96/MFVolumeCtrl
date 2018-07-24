namespace MFVolumeCtrl.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IConfig
    {
        /// <summary>
        /// 
        /// </summary>
        void Create(string filepath);
        /// <summary>
        /// 
        /// </summary>
        void Read();

        /// <summary>
        /// 
        /// </summary>
        void Write();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        void Read(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        void Write(string path);
    }
}
