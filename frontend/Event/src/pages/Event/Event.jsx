import { useState } from "react";
import CenterVert from "../../components/layots/CenterVert/CenterVert";
import styles from "./Event.module.css";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import FormatDate from "../../services/FormatDate";
import ImagesSlider from "../../components/ImageSlider/ImagesSlider";
import PrimaryButton from "../../components/buttons/PrimaryButton/PrimaryButton";
import Modal from "../../components/PopUp/Modal";
import PaticipateForm from "../../components/PaticipateForm/Paticipateform";

const Event = () => {
  const navigate = useNavigate();
  var [event, setEvent] = useState({
    id: -1,
    name: "",
    description: "",
    location: "",
    category: "",
    timeEvent: "",
    maxMembers: -1,
    urlImages: [],
  });
  const [requestError, setRequestError] = useState({
    firstNameError: "",
    secondNameError: "",
    birthDateError: "",
    common: ""
  });
  const [modalIsActive, setModalIsActive] = useState(false);
  
  const handleGetEvent = async (eventId, signal) => {
    try
    {
      var response = await axios.get(`https://localhost:5202/api/Event/GetEventById?id=${eventId}`, {
        withCredentials: true,
        headers: {
          "Content-Type": "application/json"
        },
        signal: signal
      });

      if(response.data.statusCode == 200)
      {
        setEvent({... response.data.data});
      }
      else if(response.data.statusCode == 404)
      {
        navigate("/notfound");
      }
      else
      {
        console.error("Error Get Event - " + response.data.description);
      }
    }
    catch
    {
      console.error("Get Event Error");
    }
  }

  const handlePiticipateEvent = async (eventRequest) => {
    console.log({...eventRequest});
    setRequestError({
      firstNameError: "",
      secondNameError: "",
      birthDateError: "",
      common: ""
    });
    let isValidForm = true;
    
    if(eventRequest.secondName === "" || 
      eventRequest.firstName === "")
    {
      isValidForm = false;
      setRequestError(prev => ({
        ...prev,
        secondNameError: "Is Required",
        firstName : "Is Required"
      }));
    }

    var birthDate = new Date(eventRequest.birthDate);

    if(isNaN(birthDate.getDate()))
    {
      isValidForm = false;
      setRequestError(prev => ({
        ...prev,
        birthDateError: "Invalid Signature"
      }));
    } 

    if(!isValidForm)
    {
      console.log(requestError);
      return;
    }

    try
    {
      var response = await axios.post("https://localhost:5202/api/EventMember/AddMember", {
        eventId: 0,
        firstName: eventRequest.firstName,
        secondName: eventRequest.secondName,
        email: eventRequest.email,
        birthDate: eventRequest.birthDate
      }, {
        headers: {
          "Content-Type": "application/json"
        },
        withCredentials: true
      });

      if(response.data.statusCode !== 200)
      {
        setRequestError(prev => ({
          ...prev,
          common: response.data.description
        }));
      }
      else
      {
        setModalIsActive(false);
      }

      console.log(response);
    }
    catch (error)
    {
      if(error.status == 401)
      {
        return;
      }
      console.error("Error Send Paticipation Request");
    }
  }

  useEffect(() => {
    const params = new URLSearchParams(document.location.search);
    const eventId = params.get("event");
    
    if(eventId == null)
    {
      navigate("/notfound");
    }

    const abortController = new AbortController();
    const signal = abortController.signal;

    const executeGetEvent = async () => {
      await handleGetEvent(eventId, signal);
    }

    executeGetEvent();

    return () => {
      abortController.abort();
    }
  }, []);

  return (
    <CenterVert>
      <div className={styles.Event__Main}>
        <div className={styles.Event__Images}>
          <ImagesSlider images={event.urlImages}/>
        </div>
        <div className={styles.Event__Inform}>
          <h1 className={styles.Event__Name}>{event.name}</h1>
          <section className={styles.Event__Description}>
            {event.description}
          </section>
          <p className={styles.Event__Time}>{FormatDate(event.timeEvent)}</p>
          <section className={styles.Event__ExtraInfo}>
            <p className={styles.Event__InfoKey}>location</p>
            <p className={styles.Event__InfoValue}>{event.location}</p>
          </section>
          <section className={styles.Event__ExtraInfo}>
            <p className={styles.Event__InfoKey}>category</p>
            <p className={styles.Event__InfoValue}>{event.category}</p>
          </section>
          <div className={styles.Event__Raticipate}>
            <PrimaryButton text={"Paticipate"} action={() => setModalIsActive(true)}/>
          </div>
        </div>
        <Modal isActive={modalIsActive} setIsActive={setModalIsActive}>
          <PaticipateForm 
            eventId={event.id} 
            handleSendForm={handlePiticipateEvent}
            formError={requestError}/>
        </Modal>
      </div>
    </CenterVert>
  )
}

export default Event;