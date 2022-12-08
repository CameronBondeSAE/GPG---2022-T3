using Unity.Netcode;

namespace Oscar
{
    public interface ILevelGenerate
    {
	    /// <summary>
	    /// You'll need the [ClientRpc] attribute in implementations
	    /// </summary>
        public void SpawnPerlin();

        public void SpawnBorderClientRpc();

        public void SpawnAI();

        public void SpawnItems();

        public void SpawnExplosives();
        
        public void SpawnBases();
    }
}
