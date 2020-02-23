namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        Vector3 position_;
        float orientation_;
        Vector3 velocity_;
        float rotation_;
        float time_;
        Direccion steering_;

        //-----------------------------------------------------------------------
        // CINEMATICO
        private void Start() {
            
        }
        private void Update() {
            // Modificar la posicion y la orientacion
            float half_t_sq = 0.5f * time_ * time_;
            position_ += velocity_ * time_ + steering_.lineal * half_t_sq;
            orientation_ += rotation_ * time_ + steering_.angular * half_t_sq;

            // y la velocidad y la rotacion
            velocity_ += steering_.lineal * time_;
            rotation_ += steering_.angular * time_;
        }
    }
}