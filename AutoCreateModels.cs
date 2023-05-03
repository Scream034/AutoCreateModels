using Godot;

public partial class AutoCreateModels : Node3D
{
    public string DirectoryObjPath = "assets/models/obj/";
    public string DirectoryScenePath = "scenes/prefabs/";
    public string MeshInstance3DName = "model";
    public string CollisionShape3DName = "shape";

    public override void _Ready()
    {
        var ObjFiles = System.IO.Directory.GetFiles(DirectoryObjPath, "*.obj", System.IO.SearchOption.AllDirectories);
        foreach (var fileName in ObjFiles)
        {
            var File = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
            var StaticBody = new StaticBody3D();
            var Model = new MeshInstance3D();
            var Shape = new CollisionShape3D();

            StaticBody.Name = fileName.GetFile().GetBaseName();
            Model.Name = MeshInstance3DName;
            Shape.Name = CollisionShape3DName;

            Model.Mesh = GD.Load<Mesh>(fileName);
            Shape.Shape = Model.Mesh.CreateTrimeshShape();

            StaticBody.AddChild(Model);
            StaticBody.AddChild(Shape);
			Model.Owner = StaticBody;
			Shape.Owner = StaticBody;

            var Scene = new PackedScene();
            Scene.Pack(StaticBody);

            ResourceSaver.Save(Scene, DirectoryScenePath + fileName.GetFile().GetBaseName() + ".tscn", ResourceSaver.SaverFlags.None);
        }
    }
}