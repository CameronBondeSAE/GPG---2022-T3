namespace Oscar
{
    public interface ILevelGenerate
    {
        public void SpawnPerlin();

        public void SpawnBorder();

        public void SpawnAI();

        public void SpawnItems();

        public void SpawnExplosives();
        public void SpawnBases();
    }
}
