export const getGreeting = async () => {
  const res = await fetch("/api/hello");
  return res.json();
};

// dog services
export const getDogList = async () => {
  const res = await fetch("/api/dogs");
  return res.json();
}

export const getDogById = async (id) => {
  const res = await fetch(`/api/dogs/${id}`);
  return res.json();
}


export const deleteDog = async (id) => {
  const res = await fetch(`/api/dogs/${id}`, {
    method: "DELETE"
  })
}
export const addDog = async (dogFormData) => {
  const response = await fetch("/api/dogs", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(dogFormData)
  });
  return await response.json();
}


// city services
export const getCityList = async () => {
  const res = await fetch("/api/cities");
  return res.json();
}
export const deleteCity = async (id) => {
  const res = await fetch(`/api/cities/${id}`, {
    method: "DELETE"
  })
}


//walker services
export const getWalkerList = async () => {
  const res = await fetch("/api/walkers");
  return res.json();
}

export const getWalkerByCityList = async (cityid) => {
  const res = await fetch(`/api/walkers/${cityid}`);
  return res.json();
}

export const deleteWalker = async (id) => {
  const res = await fetch(`/api/walkers/${id}`, {
    method: "DELETE"
  })
}

