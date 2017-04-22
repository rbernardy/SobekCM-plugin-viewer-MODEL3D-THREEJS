
var container;
var camera, controls, scene, renderer;
var lighting, ambient, keyLight, fillLight, backLight;
var windowHalfX = window.innerWidth / 2;
var windowHalfY = window.innerHeight / 2;

function setupTHREEJS(url_base,url_obj,url_mtl)
{
	if (!Detector.webgl)
	{
		Detector.addGetWebGLMessage();
	}

	init(url_base,url_obj,url_mtl);
	animate();
}

function init(url_base,url_obj,url_mtl) 
{
	// camera
	camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 1000);
	camera.position.z = 5;
	
	// scene
	scene = new THREE.Scene();
	ambient = new THREE.AmbientLight(0xffffff, 1.0);
	scene.add(ambient);

	// lighting
	keyLight = new THREE.DirectionalLight(new THREE.Color('hsl(30, 100%, 75%)'), 1.0);
	keyLight.position.set(-100, 0, 100);

	fillLight = new THREE.DirectionalLight(new THREE.Color('hsl(240, 100%, 75%)'), 0.75);
	fillLight.position.set(100, 0, 100);

	backLight = new THREE.DirectionalLight(0xffffff, 1.0);
	backLight.position.set(100, 0, -100).normalize();

	scene.add(keyLight);
	scene.add(fillLight);
	scene.add(backLight);

	var mtlLoader = new THREE.MTLLoader();
	mtlLoader.setBaseUrl(url_base);
	mtlLoader.setPath(url_base);

	mtlLoader.load(url_mtl, function (materials) 
	{
		materials.preload();

		materials.needsUpdate=true;
		
	    //materials.materials.default.map.magFilter = THREE.NearestFilter;
		//materials.materials.default.map.minFilter = THREE.LinearFilter;

    	var objLoader = new THREE.OBJLoader();

    	objLoader.setMaterials(materials);
	    objLoader.setPath(url_base);

	    objLoader.load(url_obj, function (object) 
	    {
			scene.add(object);
	    });
	});

	renderer = new THREE.WebGLRenderer();
	renderer.setPixelRatio(window.devicePixelRatio);
	renderer.setSize(window.innerWidth, window.innerHeight);
	renderer.setClearColor(new THREE.Color("hsl(0, 0%, 10%)"));

	container=document.getElementById("threejsdiv");
	container.appendChild(renderer.domElement);

	controls = new THREE.OrbitControls(camera, renderer.domElement);
	controls.enableDamping = true;
	controls.dampingFactor = 0.25;
	controls.enableZoom = true;
}


function render() 
{
	requestAnimationFrame(render);
    controls.update();
    renderer.render(scene, camera);
}

function animate()
{
	requestAnimationFrame(animate);
	controls.update();
	render();
}