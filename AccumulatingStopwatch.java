//package gov.usps.util.time;

import java.time.Clock;
import java.time.Duration;
import java.time.Instant;
import java.util.Collection;
import java.util.Comparator;
import java.util.LinkedHashMap;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.google.common.base.Strings;
import com.google.common.collect.ImmutableList;

public class AccumulatingStopwatch {
  private static final Logger logger = LoggerFactory.getLogger(AccumulatingStopwatch.class);
  private final LinkedHashMap<String,Timing> taskAccumulator = new LinkedHashMap<>();
  private Instant overallStartTime = null;
  private Instant latestOperationTime = null;
  private Clock clock;

  public AccumulatingStopwatch(Clock clock) {
    this.clock = clock != null ? clock : Clock.systemDefaultZone();
  }
  public AccumulatingStopwatch() {
    this(null);
  }

  public Instant start(String task) {
    logger.debug("starting task {}", task);
    Timing t = taskAccumulator.computeIfAbsent(task, k -> new Timing(k));
    if (t.currentStart != null) {
      throw new IllegalStateException("timer for '" + task + "' already running");
    }
    t.currentStart = Instant.now(clock);
    taskAccumulator.put(task, t);

    if (overallStartTime == null) {
      overallStartTime = t.currentStart;
    }
    latestOperationTime = t.currentStart;
    return t.currentStart;
  }

  public Duration stop(String task) {
    logger.debug("stopping task {}", task);
    Timing t = taskAccumulator.get(task);
    if (t == null || t.currentStart == null) {
      throw new IllegalStateException("timer for '" + task + "' not running");
    }
    Instant now = Instant.now(clock);
    final Duration duration = Duration.between(t.currentStart, now);
    t.currentStart = null;
    t.duration = t.duration.plus(duration);
    taskAccumulator.put(task, t);

    latestOperationTime = now;
    return t.duration;
  }

  public String shortSummary() {
    return "Accumulating Stopwatch total running time: " + getTotalTime();
  }

  public String prettyPrint() {
    return prettyPrint((t1, t2) -> 0);
  }
  public String prettyPrint(Comparator<Timing> orderBy) {
    StringBuilder b = new StringBuilder(shortSummary());
    b.append("\n");
    timings().stream()
        .sorted(orderBy)
        .forEachOrdered(
        t -> {
          b.append(Strings.padStart(formattedDuration(t.duration), 10, ' '));
          b.append(" ");
          b.append(t.task);
          b.append("\n");
        });
    return b.toString();
  }
  private static String formattedDuration(Duration d) {
    String iso8601 = d.toString();
    // ISO-8601 has "PT" on the front; I don't need that, I'm expecting durations
    if (iso8601.startsWith("PT")) {
      return iso8601.substring(2);
    }
    return iso8601;
  }
  public Collection<Timing> timings() {
    return ImmutableList.copyOf(taskAccumulator.values());
  }

  public Instant getOverallStartTime() {
    if (overallStartTime == null) {
      overallStartTime = Instant.now(clock);
    }
    return overallStartTime;
  }
  public Duration getTotalTime() {
    return Duration.between(overallStartTime, latestOperationTime);
  }
  public Instant getLatestOperationTime() {
    return latestOperationTime;
  }


  public static class Timing {
    private Duration duration = Duration.ZERO;
    private Instant currentStart;
    private String task;

    public Timing(String task) {
      this.task = task;
    }
    public boolean isRunning() {
      return currentStart != null;
    }
    public static Comparator<Timing> byDuration() {
      return (t1, t2) -> t1.duration.compareTo(t2.duration);
    }
    public String getTask() {
      return task;
    }
    public Instant getCurrentStart() {
      return currentStart;
    }
    public Duration getDuration() {
      return duration;
    }
  }
}
