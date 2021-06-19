
using UnityEngine;

namespace Assets.script.Mir.map
{
    public interface MapCellControllerListener
    {
        void update(int mapX, int mapY, GameObject gameObject);
    }
}
