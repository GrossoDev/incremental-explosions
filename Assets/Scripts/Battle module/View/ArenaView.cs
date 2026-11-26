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

            CreateFloor(width, height);
            FitCameraToArena(width, height);
        }

        private void CreateFloor(int width, int height)
        {
            Vector3 center = new Vector3((width - 1) / 2f, 0f, (height - 1) / 2f);
            GameObject floor = Instantiate(tilePrefab, center, Quaternion.identity, tilesParent);
            floor.transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
        }

        private void FitCameraToArena(int width, int height)
        {
            GameObject cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
            Camera camera = cameraGO.GetComponent<Camera>();

            camera.transform.position = new Vector3(width / 2f, 0, height / 2f) + cameraOffset;
            camera.orthographicSize = Mathf.Max(width / 2f, height / 2f);
        }
    }
}