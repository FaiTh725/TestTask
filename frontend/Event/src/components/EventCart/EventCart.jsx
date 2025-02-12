import styles from "./EventCart.module.css";
import FormatDate from "../../services/FormatDate";

const EventCart = ({event}) => {
  return (
    <div className={styles.EventCart__Main}>
      <div className={styles.EventCart__Image}>
        img here
      </div>
      <div className={styles.EventCart__Info}>
        <p>{event.name}</p>
        <p className={styles.EventCart__Description}>{event.description}</p>
        <p className={styles.EventCart__EventTime}>{FormatDate(event.timeEvent)}</p>
      </div>
    </div>
  ); 
}

export default EventCart;