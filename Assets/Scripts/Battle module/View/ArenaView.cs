using Battle.Presentation;
using UnityEngine;

namespace Battle.View
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] GameObject tilePrefab;
        [SerializeField] Transform tilesParent;
        [SerializeField] private Vector3 cameraOffset;

        private ArenaPresenter presenter;

        public void SetPresenter(ArenaPresenter arenaPresenter)
        {
            presenter = arenaPresenter;
        }

        public void Initialize()
        {
            int width = presenter.GetWidth();
            int height = presenter.GetHeight();

            CreateTileFloor(width, height);
            FitCameraToArena(width, height);
        }

        private void CreateTileFloor(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(x, 0, y);
                    Instantiate(tilePrefab, position, Quaternion.identity, tilesParent);
                }
            }
        }

        private void FitCameraToArena(int width, int height)
        {
            GameObject cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
            Camera camera = cameraGO.GetComponent<Camera>();

            camera.transform.position = new Vector3(width / 2, 0, height / 2) + cameraOffset;
            camera.orthographicSize = Mathf.Max(width / 2, height / 2);
        }
    }
}