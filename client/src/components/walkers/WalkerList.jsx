import { useEffect, useState } from "react";
import { Card } from 'react-bootstrap';
import { Link, Route } from "react-router-dom";
import { deleteWalker, getCityList, getWalkerByCityList, getWalkerList } from "../../scripts/apiManager";


export default function Walkers() {
  const [walkerList, setWalkerList] = useState([])
  const [cityList, setCityList] = useState([])
  const [citySelection, setCitySelection] = useState()

  useEffect(() => {
    getWalkerList().then((walkerListDTOs) => {
      setWalkerList(walkerListDTOs)
    })
  }, []);

  useEffect(() => {
    getCityList().then((cityListDTOs) => {
      setCityList(cityListDTOs)
    })
  }, []);

  const handleCitySelection = (event) => {
    const selectedCityId = event.target.value;
    setCitySelection(selectedCityId);
    if (selectedCityId) {
        getWalkerByCityList(selectedCityId).then(setWalkerList);
    } else {
        getWalkerList().then(setWalkerList);
    }
}
const handleDelete = async(id) => {
  await deleteWalker(id);
  if (citySelection) {
      getWalkerByCityList(citySelection).then(setWalkerList);
  } else {
      getWalkerList().then(setWalkerList);
  }
}

  return (
    <>
      <h2>Walkers</h2>
      <select onChange={handleCitySelection}>
      <option value="">Filter By City (All Cities)</option>
  {cityList.map(c => (
    <option key={c.id} value={c.id}>{c.name}</option>
  ))}
</select>
      {walkerList.map(w => (
        <Card key={w.id} value="">
          <Card.Body>
            <Link to={`/walkers/${w.id}`}>
              {w.name}
            </Link>
          </Card.Body>
          <button onClick={() => handleDelete(w.id)}>Delete</button>
        </Card>
      ))}
      <Link to="/walkerform">
      <button>
        Add Walker
        </button>
        </Link>
    </>
  )
}