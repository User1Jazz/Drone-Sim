# Drone Sensor Data

# Infrared Sensor data
float32 height # Single proximity value for height

float32 battery # Battery percentage

# Imu data
# Accelerations should be in m/s^2 (not in g's), and rotational velocity should be in rad/sec
geometry_msgs/Quaternion orientation
geometry_msgs/Vector3 angular_velocity
geometry_msgs/Vector3 linear_acceleration
geometry_msgs/Vector3 world_position
geometry_msgs/Vector3 local_position

# Camera data
uint8[] camera_image # Compressed image buffer