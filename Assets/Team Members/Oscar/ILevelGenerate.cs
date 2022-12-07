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

        public void SpawnAIClientRpc();

        public void SpawnItemsClientRpc();

        public void SpawnExplosivesClientRpc();
        
        public void SpawnBasesClientRpc();
    }
}
