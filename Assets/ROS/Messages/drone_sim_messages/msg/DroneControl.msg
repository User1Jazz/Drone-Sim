# ROS2 Message for Drone Control
geometry_msgs/Twist twist
geometry_msgs/Vector3 speed # x = roll/nick speed, y = yaw speed, z = throttle (up/down) speed

# Description below is the definition of the twist message
#   Vector3 linear
#    float64 x  # Forward/Backward
#    float64 y  # Left/Right
#    float64 z  # Up/Down
#  Vector3 angular
#    float64 x  # Roll
#    float64 y  # Pitch
#    float64 z  # Yaw (Yaw left/Yaw right)