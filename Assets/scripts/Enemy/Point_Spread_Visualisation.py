import matplotlib.pyplot as plt
import numpy as np

# Učitavanje podataka iz fajla
file_path = r"C:\Users\User\Desktop\vuk\programiranje\Unity\testing base-defender survival 3d\Assets\scripts\Enemy\spawn_points.txt"
points = np.loadtxt(file_path, delimiter=",")

print(points)

# Ekstrakcija X i Z koordinata (preskačemo Y jer je uvek 0)
y, x = points[:, 0], points[:, 1]  # Z sada koristimo kao Y osu

# Kreiranje 2D scatter plot-a
plt.figure(figsize=(8, 6))
plt.scatter(x, y, c='red', marker='o', label="Spawn Points")

# Postavljanje oznaka
plt.xlabel("X Axis")  # X ostaje horizontalna osa
plt.ylabel("Z Axis")  # Z koristimo kao vertikalnu osu
plt.title("2D Spawn Points Visualization (X vs Z)")
plt.legend()
plt.grid(True)

# Prikaz grafika
plt.show()

with open(r"C:\Users\User\Desktop\vuk\programiranje\Unity\testing base-defender survival 3d\Assets\scripts\Enemy\spawn_points.txt", "w") as file:
    file.write("")
